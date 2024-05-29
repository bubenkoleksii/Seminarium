import { CurrentTab } from '../constants';
import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import { immer } from 'zustand/middleware/immer';

interface AdminStore {
  currentTab: CurrentTab;
  setCurrentTab: (tab: CurrentTab) => void;
}

export const useAdminStore = create<AdminStore>()(
  immer(
    devtools(
      (set) => ({
        currentTab: CurrentTab.JoiningRequest,
        setCurrentTab: (tab: CurrentTab) =>
          set((state) => {
            state.currentTab = tab;
          }),
      }),
      { name: 'Admin store' },
    ),
  ),
);
