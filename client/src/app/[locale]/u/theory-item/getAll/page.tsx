import { GetAllTheoryLessonItems } from '@/features/user';
import { FC } from 'react';

type GetAllTheoryItemsPageProps = {
  searchParams: {
    lessonId: string;
    courseId: string;
  };
};

const GetAllTheoryItemsPage: FC<GetAllTheoryItemsPageProps> = ({
  searchParams,
}) => {
  return (
    <div className="p-3">
      <GetAllTheoryLessonItems
        lessonId={searchParams.lessonId}
        courseId={searchParams.courseId}
      />
    </div>
  );
};

export default GetAllTheoryItemsPage;
