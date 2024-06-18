import { GetTeacherAll } from '@/features/user';
import { FC } from 'react';

type TeacherAllPageProps = {
  searchParams: {
    itemId: string;
    take?: string;
    page?: string;
  };
};

const TeacherAllPage: FC<TeacherAllPageProps> = ({ searchParams }) => {
  return (
    <div className="p-3">
      <GetTeacherAll
        itemId={searchParams.itemId}
        limitParameter={searchParams.take ? Number(searchParams.take) : null}
        pageParameter={searchParams.page ? Number(searchParams.page) : null}
      />
    </div>
  );
};

export default TeacherAllPage;
