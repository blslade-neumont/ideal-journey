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
import { renderPage } from './render/render-page';

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
        
        app.get('/', (req, res) => {
            res.status(200).send(`You've reached the api server!`);
        });
        
        app.get('/register', (req, res) => {
            res.status(200).send(renderPage('Register', `
<p>
    Enter your username and password to register.
</p>
<form method="post" action="/api/register">
    <div class="form-group">
        <label for="username">Username</label>
        <input type="text" class="form-control" autofocus="autofocus" name="username" />
    </div>
    <div class="form-group">
        <label for="password">Password</label>
        <input type="password" class="form-control" name="password" />
    </div>
    <div class="form-group">
        <button type="submit" class="btn btn-primary">Submit</button>
    </div>
</form>
            `.trim()));
        });
        
        app.get('/api/highscores', async (req: Request, res: Response) => {
            let topUsers = await Users.find().sort('bestScore', -1).limit(10).toArray();
            let topScores = topUsers.map(user => ({ displayName: user.displayName, bestScore: user.bestScore }));
            res.status(200).json(topScores);
        });
        
        app.post('/api/register', async (req: Request, res: Response) => {
            let username: string = req.body.username;
            let password: string = req.body.password;
            res.status(500).send(`Hit /api/register with username: "${username}" and password: "${password}"`);
        });
        
        const server = app.listen(port, (err: any, result: any) => {
            if (err) return void(reject(err));
            resolve(server);
        });
    });
}
