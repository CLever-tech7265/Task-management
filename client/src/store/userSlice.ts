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
  role: string;
  token: string;
  shifts: any[];
  profileCompleted: boolean; // ✅ הוסף את זה
}

const initialState: UserState = {
  firstName: '',
  lastName: '',
  identity: '',
  birthDate: '',
  email: '',
  isAuthenticated: false,
  role: '',       // חדש
  token: '',      // חדש
  shifts: [],
  profileCompleted:false
};

 const userSlice = createSlice({
  name: 'user',

  initialState,

  reducers: {

    setUser(state, action: PayloadAction<Partial<UserState>>) {
  const payload = action.payload;

  if (payload.firstName !== undefined) state.firstName = payload.firstName;
  if (payload.lastName !== undefined) state.lastName = payload.lastName;
  if (payload.identity !== undefined) state.identity = payload.identity;
  if (payload.birthDate !== undefined) state.birthDate = payload.birthDate;
  if (payload.email !== undefined) state.email = payload.email;

  if (payload.isAuthenticated !== undefined)
    state.isAuthenticated = payload.isAuthenticated;

  if (payload.role !== undefined)
    state.role = payload.role;

  if (payload.token !== undefined)
    state.token = payload.token;

  if (payload.shifts)
    state.shifts = payload.shifts;
  if (payload.profileCompleted !== undefined)
    state.profileCompleted = payload.profileCompleted;
},
setShifts(
  state,
  action: PayloadAction<string[]>
) {
  state.shifts = action.payload;
},
    logout(state) {
  state.firstName = '';
  state.lastName = '';
  state.identity = '';
  state.birthDate = '';
  state.email = '';
  state.isAuthenticated = false;

  state.role = '';
  state.token = '';
  state.shifts = [];
}
  }
});

export const {
  setUser,
  setShifts,
  logout
} = userSlice.actions;

export default userSlice;