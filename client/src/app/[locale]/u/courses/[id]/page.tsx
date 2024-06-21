import { OneCourse } from '@/features/user';
import { FC } from 'react';

type OneCoursePageProps = {
  params: {
    id: string;
  };
};

const OneCoursePage: FC<OneCoursePageProps> = ({ params }) => {
  return (
    <div className="p-3">
      <OneCourse id={params.id} />
    </div>
  );
};

export default OneCoursePage;
