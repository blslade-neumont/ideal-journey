import { Collection } from 'mongodb';

import { provideUsers } from './user';
export * from './user';

export const provideFunctions: [string, (coll: Collection<any>) => Promise<void>][] = [
    ['users', provideUsers]
];
