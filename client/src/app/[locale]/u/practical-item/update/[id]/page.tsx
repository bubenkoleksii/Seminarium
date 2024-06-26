import { UpdatePracticalLessonItemForm } from '@/features/user';
import { UpdatePracticalLessonItemRequest } from '@/features/user/types/practicalLessonItemTypes';
import { FC } from 'react';

type UpdatePracticalItemPageProps = {
  params: {
    id: string;
  }
  searchParams: UpdatePracticalLessonItemRequest;
};

const UpdatePracticalItemPage: FC<UpdatePracticalItemPageProps> = ({ params, searchParams }) => {
  return (
    <div className="p-3">
      <UpdatePracticalLessonItemForm
        id={params.id}
        practicalLessonItem={searchParams}
      />
    </div>
  )
}

export default UpdatePracticalItemPage;
