'use client';

import { CreatePracticalLessonItemSubmit } from '@/features/user';
import { FC } from 'react';

type CreatePracticalItemSubmitPageProps = {
  searchParams: {
    itemId: string;
  };
};

const CreatePracticalItemSubmitPage: FC<CreatePracticalItemSubmitPageProps> = ({
  searchParams,
}) => {
  return (
    <div className="p-3">
      <CreatePracticalLessonItemSubmit itemId={searchParams.itemId} />
    </div>
  );
};

export default CreatePracticalItemSubmitPage;
