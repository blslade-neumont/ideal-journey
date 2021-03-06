import * as express from 'express';
import { Request, Response } from 'express';
import { Server } from 'http';
import { config } from './config';
import { User, createAuthToken, Users, sanitizeUser } from './models/user';
import { wrapPromise } from './util/wrap-promise';
import cookieParser = require('cookie-parser');
import bodyParser = require('body-parser')
import { allowAllOrigins } from './util/allow-all-origins';
import { parseUser } from './util/parse-user';
import { registerUser } from './util/register-user';
import { loginAsUser } from './util/login-as-user';
import { verifyAuthToken } from './util/verify-auth-token';
import path = require('path');

export function initializeRoutesAndListen(port: number): Promise<Server> {
    return new Promise((resolve, reject) => {
        let secure = config.try('server.secure', false);
        
        let app = express();
        
        app.use(
            cookieParser(),
            bodyParser.urlencoded({ extended: true }),
            bodyParser.json(),
            allowAllOrigins
        );
        
        app.set('view engine', 'pug');
        app.set('views', path.join(__dirname, '../views'));
        
        app.get('/', (req, res) => {
            res.status(200).render('index', { title: 'Homepage' });
        });
        
        app.post('/poke', async (req: Request, res: Response) => {
            res.status(200).send("I'm awake, I'm awake!");
        });
        
        app.post('/login', async (req: Request, res: Response) => {
            let username: string = req.body.username;
            let password: string = req.body.password;
            try {
                let loginResponse = await loginAsUser(username, password);
                res.status(200).send(loginResponse);
            }
            catch (e) {
                res.status(403).json({ authToken: null, error: 'Failed to log in' });
            }
        });
        
        app.post('/verify-token', async (req: Request, res: Response) => {
            let authToken: string = req.body.authToken;
            try {
                let loginResponse = await verifyAuthToken(authToken);
                res.status(200).send(loginResponse);
            }
            catch (e) {
                res.status(403).json({ authToken: null, error: 'Failed to verify auth token.' });
            }
        });
        
        app.get('/register', (req, res) => {
            res.status(200).render('register', { title: 'Register' });
        });
        
        app.post('/register', async (req: Request, res: Response) => {
            let username: string = req.body.username;
            let password: string = req.body.password;
            try {
                let user = await registerUser(username, password);
                res.status(200).render('account-creation-successful', { title: 'Registration Successful', username: username });
            }
            catch (e) {
                res.status(200).render('register', { title: 'Register', username: username, error: 'Failed to register. Check your username and password, then try again.' });
            }
        });
        
        app.get('/api/highscores', parseUser, async (req: Request, res: Response) => {
            let topUsers = await Users.find().sort('bestScore', -1).limit(5).toArray();
            let topScores = topUsers.map(user => ({ username: user.username, bestScore: user.bestScore }));
            let scores = { highscores: topScores };
            let currentUser: User = (<any>req).jwt;
            if (currentUser) {
                (<any>scores).rank = (await Users.count({ bestScore: { $gt: currentUser.bestScore } })) + 1;
            }
            res.status(200).json(scores);
        });
        
        app.get('/api/update-highscore', parseUser, async (req: Request, res: Response) => {
            let originalUser: Partial<User> = (<any>req).jwt;
            if (!originalUser) {
                res.status(403).send(`You aren't logged in, so you can't update your highscore!`);
                return;
            }
            
            let score = req.query['score'];
            if (!score || `${+score}` !== score) {
                res.status(423).send(`You must provide a score to update your score`);
                return;
            }
            score = +score;
            
            let findBy: Partial<User> = {
                username: originalUser.username
            };
            await Users.updateOne(findBy, {
                $set: {
                    bestScore: score
                }
            });
            let user = await Users.findOne(findBy);
            
            let authToken = createAuthToken(user!);
            let sanitizedUser = JSON.parse(JSON.stringify(sanitizeUser(user!)));
            res.status(200).send(JSON.stringify({ authToken, user: sanitizedUser }));
        });
        
        const server = app.listen(port, (err: any, result: any) => {
            if (err) return void(reject(err));
            resolve(server);
        });
    });
}
