'use client';

import { useAuthRedirectByRole, useSetCurrentTab } from '@/shared/hooks';
import { FC, useState } from 'react';
import { CurrentTab, userQueries } from '../../constants';
import { useLocale, useTranslations } from 'next-intl';
import { useIsMutating, useQuery } from '@tanstack/react-query';
import { useProfiles } from '../../hooks';
import { ApiResponse } from '@/shared/types';
import { getOne } from '../../api/schoolProfilesApi';
import { Loader } from '@/components/loader';
import { Error } from '@/components/error';
import {
  CreateSchoolProfileRequest,
  SchoolProfileResponse,
} from '../../types/schoolProfileTypes';
import { Child } from './Child';
import toast from 'react-hot-toast';
import { InputTextModal } from '@/components/modal/InputTextModal';
import { useMutation } from '@tanstack/react-query';
import { addChild } from '../../api/schoolProfilesApi';
import { userMutations } from '../../constants';
import { useSchoolProfilesStore } from '../../store/schoolProfilesStore';
import { useRouter } from 'next/navigation';
import { Button } from 'flowbite-react';

type ChildrenProps = {
  id: string;
};

const Children: FC<ChildrenProps> = ({ id }) => {
  const t = useTranslations('SchoolProfile');
  const c = useTranslations('Children');
  const v = useTranslations('Validation');

  const { replace } = useRouter();
  const activeLocale = useLocale();

  const isMutating = useIsMutating();
  const { isUserLoading } = useAuthRedirectByRole(activeLocale, 'userOnly');
  const { activeProfile, isLoading: profilesLoading } = useProfiles();

  const { data, isLoading } = useQuery<ApiResponse<SchoolProfileResponse>>({
    queryKey: ['children', id],
    queryFn: () => getOne(id),
    enabled: !!id,
    retry: userQueries.options.retry,
  });

  const clearSchoolProfiles = useSchoolProfilesStore((store) => store.clear);

  const { mutate: addNewChild } = useMutation({
    mutationFn: addChild,
    mutationKey: [userMutations.createSchoolProfile],
    retry: userMutations.options.retry,
    onSuccess: (response) => {
      if (response && response.error) {
        if (response.error.detail.includes('max_profiles_count')) {
          toast.error(t('max_profiles_count'));

          replace(`/${activeLocale}/u`);
          return;
        }

        const errorMessages = {
          400: t('badRequest'),
          409: t('alreadyExists'),
        };

        toast.error(errorMessages[response.error.status] || v('internal'));
      } else {
        clearSchoolProfiles();

        toast.success(t('createChildSuccess'), { duration: 1500 });
      }

      replace(`/${activeLocale}/u/children/${data?.id}`);
    },
  });

  const [invitationCodeModalOpen, setInvitationCodeModalOpen] = useState(false);
  const handleInvitationCodeModalOpen = () => {
    setInvitationCodeModalOpen(true);
  };
  const handleInvitationCodeModalClose = (confirmed: boolean, text: string) => {
    setInvitationCodeModalOpen(false);

    if (!confirmed) {
      return;
    }

    if (!text.includes('/u/school-profile/create/parent')) {
      toast.error(t('invalidInvitationLink'));

      return;
    } else {
      const request: CreateSchoolProfileRequest = {
        invitationCode: decodeURIComponent(text.split('parent/')[1]),
        name: data?.name,
      };

      addNewChild({
        data: request,
        schoolProfileId: data?.id,
      });
    }
  };

  useSetCurrentTab(CurrentTab.Children);

  if (isLoading || isMutating || isUserLoading || profilesLoading) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{c('title')}</h2>

        <Loader />
      </>
    );
  } else {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  }

  if (data && data.error) {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{c('title')}</h2>

        <Error error={data.error} />
      </>
    );
  }

  if (activeProfile.type !== 'parent') {
    return (
      <>
        <h2 className="mb-4 text-center text-xl font-bold">{c('title')}</h2>

        <p className="text-center text-red-700">{c('onlyParents')}</p>
      </>
    );
  }

  return (
    <div className="mb-2 p-3">
      <h2 className="mb-4 text-center text-xl font-bold">
        {c('title')}
        {data?.children?.length > 0 ? ` (${data?.children?.length})` : ``}
      </h2>

      <h6 className="text-center font-bold">
        <span className="text-purple-950 lg:text-2xl">{data?.name}</span>
      </h6>

      <InputTextModal
        open={invitationCodeModalOpen}
        text={t('invitationLabelModal')}
        required={true}
        isTextarea={true}
        onClose={handleInvitationCodeModalClose}
      />

      <div className="mt-2 flex items-center justify-center">
        <Button
          onClick={handleInvitationCodeModalOpen}
          gradientMonochrome="purple"
        >
          <span className="text-white">{t('addChild')}</span>
        </Button>
      </div>

      {data?.children?.length > 0 ? (
        <div className="flex flex-wrap justify-center">
          {data?.children?.map((c, idx) => <Child child={c} key={idx} />)}
        </div>
      ) : (
        <p className="mt-3 text-center">{c('notFound')}</p>
      )}
    </div>
  );
};

export { Children };
