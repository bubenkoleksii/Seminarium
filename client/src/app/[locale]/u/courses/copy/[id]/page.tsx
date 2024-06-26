import { CopyCourseForm } from '@/features/user';
import type { UpdateOrCopyCourseRequest } from '@/features/user/types/courseTypes';
import { FC } from 'react';

type CopyCoursePageProps = {
  params: {
    id: string;
  };
  searchParams: UpdateOrCopyCourseRequest;
};

const CopyCoursePage: FC<CopyCoursePageProps> = ({ params, searchParams }) => {
  return (
    <div className="p-3">
      <CopyCourseForm id={params.id} course={searchParams} />
    </div>
  )
}

export default CopyCoursePage;
