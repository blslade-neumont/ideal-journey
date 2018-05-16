import * as express from 'express';
import { Request, Response } from 'express';
import { Server } from 'http';
import { config } from './config';
import { User, createAuthToken, Users } from './models/user';
import { wrapPromise } from './util/wrap-promise';
import cookieParser = require('cookie-parser');
import bodyParser = require('body-parser')
import { allowAllOrigins } from './util/allow-all-origins';
import { parseUser } from './util/parse-user';
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
        
        app.get('/register', (req, res) => {
            res.status(200).render('register', { title: 'Register' });
        });
        
        app.get('/api/highscores', async (req: Request, res: Response) => {
            let topUsers = await Users.find().sort('bestScore', -1).limit(10).toArray();
            let topScores = topUsers.map(user => ({ displayName: user.displayName, bestScore: user.bestScore }));
            res.status(200).json(topScores);
        });
        
        app.post('/register', async (req: Request, res: Response) => {
            let username: string = req.body.username;
            let password: string = req.body.password;
            if (username === 'abc' && password === '123') {
                res.status(200).render('account-creation-successful', { title: 'Registration Successful', username: username });
            }
            else {
                res.status(200).render('register', { title: 'Register', username: username, error: 'Failed to register. Try again later.' });
            }
        });
        
        const server = app.listen(port, (err: any, result: any) => {
            if (err) return void(reject(err));
            resolve(server);
        });
    });
}
