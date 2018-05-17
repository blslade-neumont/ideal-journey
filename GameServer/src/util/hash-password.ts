import bcrypt = require('bcrypt');

const SALT_PASSWORD_ROUNDS = 12;

export function hashPassword(password: string) {
    return bcrypt.hash(password, SALT_PASSWORD_ROUNDS);
}
