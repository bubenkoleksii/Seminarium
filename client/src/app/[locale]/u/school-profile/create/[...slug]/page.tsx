'use client';

import { FC } from 'react';
import {
  CreateDefaultSchoolProfile,
  CreateParentSchoolProfile,
  CreateStudentSchoolProfile,
  CreateTeacherSchoolProfile,
} from '@/features/user';

type Props = {
  params: {
    slug: string[];
  };
};

const CreateProfilePage: FC<Props> = ({ params }) => {
  const [type, invitationCode] = params.slug.map((param) =>
    decodeURIComponent(param),
  );

  const createForms = {
    'school_admin': (
      <CreateDefaultSchoolProfile type={type} invitationCode={invitationCode} />
    ),
    'class_teacher': (
      <CreateDefaultSchoolProfile type={type} invitationCode={invitationCode} />
    ),
    'student': (
      <CreateStudentSchoolProfile type={type} invitationCode={invitationCode} />
    ),
    'parent': (
      <CreateParentSchoolProfile type={type} invitationCode={invitationCode} />
    ),
    'teacher': (
      <CreateTeacherSchoolProfile type={type} invitationCode={invitationCode} />
    ),
  };

  return createForms[type] || null;
};

export default CreateProfilePage;
