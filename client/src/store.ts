// store.ts
import { configureStore, createSlice} from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';

interface UserState {
  username: string;
  password: string;
}

const initialState: UserState = {
  username: '',
  password: ''
};

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    setUser(state, action: PayloadAction<UserState>) {
      state.username = action.payload.username;
      state.password = action.payload.password;
    }
  }
});

export const { setUser } = userSlice.actions;

export const store = configureStore({
  reducer: {
    user: userSlice.reducer
  }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;