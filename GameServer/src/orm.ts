import { MongoClient } from 'mongodb';
import { config } from './config';
import { parallel, AsyncFunction } from 'async';
import { provideFunctions } from './models';

type CallbackT = (err?: any, result?: any) => void;

export function initializeDatabase(): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
        MongoClient.connect(config.get('db.connectionString'), (err, client) => {
            if (err) return void(reject(err));
            
            let db = client.db(config.get('db.databaseName'));
            
            let allCollectionsTasks: AsyncFunction<any, any>[] = provideFunctions.map(([name, provideFn]) => {
                return (next: CallbackT) => db.collection(name, (err, collection) => {
                    if (err) return void(next(err));
                    provideFn(collection)
                        .then(() => next(null, collection))
                        .catch(next);
                });
            });
            
            parallel(allCollectionsTasks, (err, results) => {
                if (err) return void(reject(err));
                resolve();
            });
        });
    });
}
