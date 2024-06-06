import { FC } from 'react';
import type { UpdateSchoolProfileRequest } from '@/features/user/types/schoolProfileTypes';
import {
  UpdateDefaultSchoolProfile,
  UpdateParentSchoolProfile,
  UpdateStudentSchoolProfile,
  UpdateTeacherSchoolProfile,
} from '@/features/user';

type UpdateSchoolProfileProps = {
  params: {
    id: string;
  };
  searchParams: UpdateSchoolProfileRequest
}

const UpdateSchoolProfile: FC<UpdateSchoolProfileProps> = ({
  params,
  searchParams
}) => {
  const type = searchParams.type;

  const updateForms = {
    school_admin: (
      <UpdateDefaultSchoolProfile id={params.id} schoolProfile={searchParams} />
    ),
    class_teacher: (
      <UpdateDefaultSchoolProfile id={params.id} schoolProfile={searchParams} />
    ),
    student: (
      <UpdateStudentSchoolProfile id={params.id} schoolProfile={searchParams} />
    ),
    parent: (
      <UpdateParentSchoolProfile id={params.id} schoolProfile={searchParams} />
    ),
    teacher: (
      <UpdateTeacherSchoolProfile id={params.id} schoolProfile={searchParams} />
    ),
  };

  return updateForms[type] || null;
};

export default UpdateSchoolProfile;
