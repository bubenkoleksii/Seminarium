import { UpdateCourseForm } from '@/features/user';
import type { UpdateOrCopyCourseRequest } from '@/features/user/types/courseTypes';
import { FC } from 'react';

type UpdateCoursePageProps = {
  params: {
    id: string;
  };
  searchParams: UpdateOrCopyCourseRequest;
};

const UpdateCoursePage: FC<UpdateCoursePageProps> = ({
  params,
  searchParams,
}) => {
  return (
    <div className="p-3">
      <UpdateCourseForm id={params.id} course={searchParams} />
    </div>
  );
};

export default UpdateCoursePage;
