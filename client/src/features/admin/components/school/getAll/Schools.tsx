'use client';

import { FC } from 'react';
import { useSetCurrentTab } from '@/shared/hooks';
import { CurrentTab } from '@/features/admin/constants';

const Schools: FC = () => {

  useSetCurrentTab(CurrentTab.School);

  return (
    <div>
      qwerty
    </div>
  );
};

export { Schools };
