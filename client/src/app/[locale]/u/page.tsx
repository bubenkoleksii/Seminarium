import { FC } from 'react';
import { UserProfile } from '@/features/user';

const UserPage: FC = () => {
  return (
    <div className="p-3">
      <UserProfile />
    </div>
  );
};

export default UserPage;
