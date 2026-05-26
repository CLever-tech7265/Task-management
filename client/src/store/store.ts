import {
  configureStore,
  createSlice
} from '@reduxjs/toolkit';

import type {
  PayloadAction
} from '@reduxjs/toolkit';

interface UserState {
  firstName: string;
  lastName: string;
  identity: string;
  birthDate: string;
  email: string;
  isAuthenticated: boolean;
}

const initialState: UserState = {
  firstName: '',
  lastName: '',
  identity: '',
  birthDate: '',
  email: '',
  isAuthenticated: false
};

const userSlice = createSlice({
  name: 'user',

  initialState,

  reducers: {

    setUser(
      state,
      action: PayloadAction<UserState>
    ) {
      state.firstName =
        action.payload.firstName;

      state.lastName =
        action.payload.lastName;

      state.identity =
        action.payload.identity;

      state.birthDate =
        action.payload.birthDate;

      state.email =
        action.payload.email;

      state.isAuthenticated =
        action.payload.isAuthenticated;
    },

    logout(state) {
      state.firstName = '';
      state.lastName = '';
      state.identity = '';
      state.birthDate = '';
      state.email = '';

      state.isAuthenticated = false;
    }
  }
});

export const {
  setUser,
  logout
} = userSlice.actions;

export const store = configureStore({
  reducer: {
    user: userSlice.reducer
  }
});

export type RootState =
  ReturnType<typeof store.getState>;

export type AppDispatch =
  typeof store.dispatch;