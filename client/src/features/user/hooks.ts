'use client';

import type { ApiResponse } from '@/shared/types';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { useSchoolProfilesStore } from '@/features/user/store/schoolProfilesStore';
import { useQuery } from '@tanstack/react-query';

const useProfiles = () => {
  const profilesStore = useSchoolProfilesStore();

  const { data, isLoading } = useQuery<ApiResponse<SchoolProfileResponse[]>>({
    queryFn:
  });

  return {
    isLoading
  }
}