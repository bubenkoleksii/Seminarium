import { Courses } from '@/features/user';
import { FC } from 'react';

type CoursePageProps = {
  searchParams: {
    studyPeriodId: string;
    search: string;
    take: string;
    page: string;
  };
};

const CoursesPage: FC<CoursePageProps> = ({ searchParams }) => {
  return (
    <div className="p-3">
      <Courses
        studyPeriodIdParameter={searchParams.studyPeriodId}
        searchParameter={searchParams.search}
        limitParameter={searchParams.take ? Number(searchParams.take) : null}
        pageParameter={searchParams.page ? Number(searchParams.page) : null}
      />
    </div>
  );
};

export default CoursesPage;
