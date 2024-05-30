import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import { immer } from 'zustand/middleware/immer';

interface SchoolProfilesStore {
  activeProfile: SchoolProfileResponse,
  profiles: SchoolProfileResponse[],
  setActiveProfile: (profile: SchoolProfileResponse) => void;
  setProfiles: (profile: SchoolProfileResponse[]) => void;
}

export const useSchoolProfilesStore = create<SchoolProfilesStore>()(
  immer(
    devtools(
      (set) => ({
        activeProfile: {},
        profiles: [],
        setActiveProfile: (activeProfile: SchoolProfileResponse) =>
          set((state) => state.activeProfile = activeProfile),
        setProfiles: (profiles: SchoolProfileResponse[]) =>
          set((state) => state.profiles = profiles)
      }),
      { name: 'School profiles store' }
    )
  )
);
