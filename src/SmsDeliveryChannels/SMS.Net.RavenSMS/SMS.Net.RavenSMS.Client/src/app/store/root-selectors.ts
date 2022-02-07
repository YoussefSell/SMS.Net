import { MessagesStoreSelectors } from './messages-store';
import { createSelector } from '@ngrx/store';

/**
 * select global errors form all stores
 */
export const ErrorsSelector = createSelector(
    MessagesStoreSelectors.ErrorSelector,
    (error) => {
        return [
            error
        ];
    }
);
