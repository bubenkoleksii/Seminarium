import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import { immer } from 'zustand/middleware/immer';

interface SchoolProfilesStore {
  activeProfile: SchoolProfileResponse;
  profiles: SchoolProfileResponse[];
  setProfiles: (profile: SchoolProfileResponse[]) => void;
  changeActiveProfile: (id: string) => void;
}

export const useSchoolProfilesStore = create<SchoolProfilesStore>()(
  immer(
    devtools(
      (set) => ({
        activeProfile: null,
        profiles: [],
        setProfiles: (profiles: SchoolProfileResponse[]) =>
          set((state) => {
            state.profiles = profiles;
            state.activeProfile = profiles.find((profile) => profile.isActive);
          }),
        changeActiveProfile: (id: string) =>
          set((state) => {
            state.profiles = state.profiles.map(
              (profile) => (profile.isActive = profile.id === id),
            );
            state.activeProfile = state.profiles.find(
              (profile) => profile.isActive,
            );
          }),
      }),
      { name: 'School profiles store' },
    ),
  ),
);
