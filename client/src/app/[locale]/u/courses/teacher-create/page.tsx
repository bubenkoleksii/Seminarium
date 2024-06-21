import { CreateCourseTeacher } from '@/features/user';
import { FC } from 'react';

type CreateTeacherPageProps = {
  searchParams: {
    courseId: string;
    schoolId: string;
  };
};

const CreateTeacherPage: FC<CreateTeacherPageProps> = ({ searchParams }) => {
  return (
    <div className="p-3">
      <CreateCourseTeacher
        courseId={searchParams.courseId}
        schoolId={searchParams.schoolId}
      />
    </div>
  );
};

export default CreateTeacherPage;
