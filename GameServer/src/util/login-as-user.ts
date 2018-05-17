import { hashPassword } from "./hash-password";
import { User, Users, createAuthToken } from "../models/user";
import bcrypt = require('bcrypt-nodejs');

export async function loginAsUser(username: string, password: string): Promise<string> {
    if (!username || !password) throw new Error(`Can't login with no username or password.`);
    
    let findBy: Partial<User> = {
        username: username
    };
    
    let user = await Users.findOne(findBy);
    if (!user) throw new Error(`Failed to login as ${username}`);
    
    let samePassword = bcrypt.compareSync(password, user.passwordHash);
    if (!samePassword) throw new Error(`Failed to login as ${username}`);
    
    let authToken = createAuthToken(user);
    return authToken;
}
