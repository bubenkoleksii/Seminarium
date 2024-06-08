import { CurrentTab } from '@/features/user/constants';
import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import { immer } from 'zustand/middleware/immer';

interface UserStore {
  currentTab: CurrentTab;
  setCurrentTab: (tab: CurrentTab) => void;
}

export const useUserStore = create<UserStore>()(
  immer(
    devtools(
      (set) => ({
        currentTab: CurrentTab.Profile,
        setCurrentTab: (tab: CurrentTab) =>
          set((state) => {
            state.currentTab = tab;
          }),
      }),
      { name: 'User store' },
    ),
  ),
);
