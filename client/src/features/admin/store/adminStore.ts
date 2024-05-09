import { CurrentTab } from '@/features/admin/types';
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
        currentTab: CurrentTab.Profile,
        setCurrentTab: (tab: CurrentTab) => set({ currentTab: tab }),
      }),
      { name: 'Admin store' },
    ),
  ),
);
