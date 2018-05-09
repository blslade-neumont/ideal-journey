import { Collection } from 'mongodb';
import cloneDeep = require('lodash.clonedeep');
import * as jwt from 'jsonwebtoken';
import { config } from '../config';

const EXPIRES_IN_DAYS = 2;

export interface User {
    _id: string;
    displayName: string;
    bestScore: number;
};

export function sanitizeUser(user: User): Partial<User> {
    let clone = cloneDeep(user);
    //TODO: delete properties that should not be sent to the client
    //(Ex: password hashes)
    return clone;
}

export function createAuthToken(user: User): string {
    let secret = config.try<string>('jwt.secret');
    if (!secret) throw new Error(`Cannot create auth token: there is no JWT secret`);
    let sanitizedUser = JSON.parse(JSON.stringify(sanitizeUser(user)));
    return jwt.sign(sanitizedUser, secret, { expiresIn: `${EXPIRES_IN_DAYS} days` });
}

export let Users: Collection<User>;
export function provideUsers(users: Collection<User>) {
    Users = users;
}
