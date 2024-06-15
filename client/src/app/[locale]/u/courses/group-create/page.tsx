import { CreateCourseGroup } from '@/features/user';
import { FC } from 'react';

type CreateCourseGroupPageProps = {
  searchParams: {
    courseId: string;
    schoolId: string;
  };
};

const CreateCourseGroupPage: FC<CreateCourseGroupPageProps> = ({
  searchParams,
}) => {
  return (
    <div className="p-3">
      <CreateCourseGroup
        courseId={searchParams.courseId}
        schoolId={searchParams.schoolId}
      />
    </div>
  );
};

export default CreateCourseGroupPage;
