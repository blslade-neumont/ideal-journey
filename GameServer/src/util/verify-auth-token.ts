import { hashPassword } from "./hash-password";
import { User, Users, createAuthToken, sanitizeUser } from "../models/user";
import bcrypt = require('bcrypt-nodejs');
import { parseAuthToken } from "./parse-auth-token";

export async function verifyAuthToken(originalAuthToken: string): Promise<string> {
    if (!originalAuthToken) throw new Error(`Can't verify falsy auth token.`);
    
    let originalUser = parseAuthToken(originalAuthToken);
    if (!originalUser) throw new Error(`Invalid auth token.`);
    
    let findBy: Partial<User> = {
        username: originalUser.username
    };
    
    let user = await Users.findOne(findBy);
    if (!user) throw new Error(`Failed to re-login as ${originalUser.username}. That user no longer exists!`);
    
    let authToken = createAuthToken(user);
    let sanitizedUser = JSON.parse(JSON.stringify(sanitizeUser(user)));
    return JSON.stringify({ authToken, user: sanitizedUser });
}
