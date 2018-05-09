

module.exports = {
    server: {
        port: process.env.PORT || 80
    },
    
    db: {
        connectionString: process.env.API_MONGO_CONNECTION_STRING,
        databaseName: process.env.API_MONGO_DATABASE_NAME || 'turbo-winner'
    },
    
    jwt: {
        secret: process.env.JWT_SECRET
    }
};
