import { GetStudentAll } from '@/features/user';
import { FC } from 'react';

type StudentAllPageProps = {
  searchParams: {
    studentId: string;
    take?: string;
    page?: string;
  }
}

const StudentAllPage: FC<StudentAllPageProps> = ({ searchParams }) => {
  return (
    <div className="p-3">
      <GetStudentAll
        studentId={searchParams.studentId}
        limitParameter={searchParams.take ? Number(searchParams.take) : null}
        pageParameter={searchParams.page ? Number(searchParams.page) : null}
      />
    </div>
  )
}

export default StudentAllPage;
