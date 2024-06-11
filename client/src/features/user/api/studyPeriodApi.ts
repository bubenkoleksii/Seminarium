'use server';

import { api } from '@/shared/api';
import type { ApiResponse } from '@/shared/types';
import { studyPeriods } from '../routes';
import type {
  CreateStudyPeriodRequest,
  StudyPeriodResponse,
  UpdateStudyPeriodRequest,
} from '../types/studyPeriodTypes';

type GetAll = () => Promise<ApiResponse<StudyPeriodResponse[]>>;

type Create = ({
  data,
  schoolProfileId,
}: {
  data: CreateStudyPeriodRequest;
  schoolProfileId: string;
}) => Promise<ApiResponse<StudyPeriodResponse>>;

type Update = ({
  data,
  schoolProfileId,
}: {
  data: UpdateStudyPeriodRequest;
  schoolProfileId: string;
}) => Promise<ApiResponse<StudyPeriodResponse>>;

type Remove = ({
  id,
  schoolProfileId,
}: {
  id: string;
  schoolProfileId: string;
}) => Promise<ApiResponse<any>>;

export const getAllStudyPeriods: GetAll = () => api.get(studyPeriods.getAll);

export const createStudyPeriod: Create = ({ data, schoolProfileId }) =>
  api.create(studyPeriods.create, data, false, schoolProfileId);

export const updateStudyPeriod: Update = ({ data, schoolProfileId }) =>
  api.update(studyPeriods.update, data, false, schoolProfileId);

export const removeStudyPeriod: Remove = ({ id, schoolProfileId }) =>
  api.remove(studyPeriods.remove(id), schoolProfileId);
