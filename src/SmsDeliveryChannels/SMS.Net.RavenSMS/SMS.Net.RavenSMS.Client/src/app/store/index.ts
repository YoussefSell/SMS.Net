import { RootStoreModule } from './root-store.module';
import * as RootStoreSelectors from './root-selectors';
import * as RootStoreState from './root-state';
import * as RootActions from './root-actions';

// export root store
export { RootStoreState, RootStoreSelectors, RootActions, RootStoreModule };

// export features stores
export * from './messages-store';
export * from './state-persistence';
export * from './ui-store';
