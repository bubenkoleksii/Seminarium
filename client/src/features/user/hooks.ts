'use client';

import { useEffect, useState } from 'react';
import type { ApiResponse } from '@/shared/types';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { useSchoolProfilesStore } from '@/features/user/store/schoolProfilesStore';
import { get } from '@/features/user/api/schoolProfilesApi';

export const useProfiles = () => {
  const profilesStore = useSchoolProfilesStore();

  const [isLoading, setIsLoading] = useState(true);

  const [profiles, setProfiles] = useState<ApiResponse<SchoolProfileResponse[]>>(null);
  const [activeProfile, setActiveProfile] = useState<SchoolProfileResponse>(null);
  const [isError, setIsError] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      setIsLoading(true);
      setIsError(false);

      if (profilesStore.profiles.length > 0 && profilesStore.activeProfile) {
        setProfiles(profilesStore.profiles);
        setActiveProfile(profilesStore.activeProfile);

        setIsLoading(false);
        return;
      }

      try {
        const response = await get();

        if (response && response.error) {
          setIsError(true);
          return;
        }

        setProfiles(response);
        profilesStore.setProfiles(response);

        setActiveProfile(activeProfile);
        return;
      } catch {
        setIsError(true);
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, []);

  const currentProfile =
    activeProfile || profiles?.find(profile => profile.isActive) || null;

  return {
    isLoading,
    isError: isError && !profiles,
    profiles,
    activeProfile: currentProfile,
  };
};