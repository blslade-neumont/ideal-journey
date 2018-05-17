import bcrypt = require('bcrypt-nodejs');

export async function hashPassword(password: string): Promise<string> {
    return bcrypt.hashSync(password);
}
