import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import { immer } from 'zustand/middleware/immer';

interface SchoolProfilesStore {
  activeProfile: SchoolProfileResponse | null;
  profiles: SchoolProfileResponse[] | null;
  setProfiles: (profile: SchoolProfileResponse[]) => void;
  clear: () => void;
  changeActiveProfile: (id: string) => void;
  changeImg: (id: string, newImg: string) => void;
}

export const useSchoolProfilesStore = create<SchoolProfilesStore>()(
  immer(
    devtools(
      (set) => ({
        activeProfile: null,
        profiles: null,
        setProfiles: (profiles: SchoolProfileResponse[]) =>
          set((state) => {
            state.profiles = profiles;
            state.activeProfile = profiles.find((profile) => profile.isActive);
          }),
        changeImg: (id: string, newImg: string) =>
          set((state) => {
            const updatedProfiles = state.profiles.map((profile) => {
              if (profile.id === id) {
                return {
                  ...profile,
                  img: newImg,
                };
              }
              return profile;
            });

            return {
              profiles: updatedProfiles,
              activeProfile: updatedProfiles.find(
                (profile) => profile.isActive,
              ),
            };
          }),
        changeActiveProfile: (id: string) =>
          set((state) => {
            const updatedProfiles = state.profiles.map((profile) => {
              return {
                ...profile,
                isActive: profile.id === id,
              };
            });

            return {
              profiles: updatedProfiles,
              activeProfile: updatedProfiles.find(
                (profile) => profile.isActive,
              ),
            };
          }),
        clear: () =>
          set((state) => {
            state.profiles = null;
            state.activeProfile = null;
          }),
      }),
      { name: 'School profiles store' },
    ),
  ),
);
