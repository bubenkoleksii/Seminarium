import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import { immer } from 'zustand/middleware/immer';

interface NavStore {
  sidebarOpen: boolean;
  setSidebarOpen: (sidebarOpen: boolean) => void;
}

export const useNavStore = create<NavStore>()(
  immer(
    devtools(
      (set) => ({
        sidebarOpen: true,
        setSidebarOpen: (sidebarOpen: boolean) =>
          set((state) => {
            state.sidebarOpen = sidebarOpen;
          }),
      }),
      { name: 'Nav store' },
    ),
  ),
);
