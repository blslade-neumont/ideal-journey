import { hashPassword } from "./hash-password";
import { User, Users } from "../models/user";
import { wrapPromise } from "./wrap-promise";

export async function registerUser(username: string, password: string): Promise<User> {
    if (!username || !password) throw new Error(`Can't register with no username or password.`);
    
    let user: Partial<User> = {
        bestScore: 0,
        username: username,
        passwordHash: await hashPassword(password)
    };
    
    return await wrapPromise<User>(Users.insert.bind(Users), user);
}
