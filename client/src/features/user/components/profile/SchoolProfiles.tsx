import { FC, useState } from 'react';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { SchoolProfile } from '@/features/user/components/profile/SchoolProfile';
import { useLocale, useTranslations } from 'next-intl';
import { InputTextModal } from '@/components/modal/InputTextModal';
import { Button } from 'flowbite-react';
import { useRouter } from 'next/navigation';
import { toast } from 'react-hot-toast';

type SchoolProfilesProps = {
  profiles: SchoolProfileResponse[];
};

const SchoolProfiles: FC<SchoolProfilesProps> = ({ profiles }) => {
  const t = useTranslations('SchoolProfile');
  const { replace } = useRouter();

  const [invitationCodeModalOpen, setInvitationCodeModalOpen] = useState(false);
  const handleInvitationCodeModalOpen = () => {
    setInvitationCodeModalOpen(true);
  }
  const handleInvitationCodeModalClose = (confirmed: boolean, text: string) => {
    setInvitationCodeModalOpen(false);

    if (!confirmed) {
      return;
    }

    if (!text.includes('/u/school-profile/create/')) {
      toast.error(t('invalidInvitationLink'));

      return;
    } else {
      replace(text);
    }
  }

  if (!profiles || profiles.length === 0) {
    return (
      <div className="relative">
        <h2 className="md:text mb-2 mt-4 text-center text-sm font-semibold lg:text-xl">
          {t('listLabel')}
        </h2>
        <div className="flex flex-col flex-wrap items-center">
          <p>{t('notFound')}</p>

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
              <span className="text-white">{t('addProfileBtn')}</span>
            </Button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="relative">
      <h2 className="md:text mb-2 mt-2 text-center text-sm font-bold lg:text-xl">
        {t('listLabel')} {profiles.length > 0 ? `(${profiles.length})` : ''}
      </h2>

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
          <span className="text-white">{t('addProfileBtn')}</span>
        </Button>
      </div>

      <div className="flex flex-wrap justify-center">
        {profiles.map((profile, idx) => (
          <SchoolProfile key={idx} profile={profile} />
        ))}
      </div>
    </div>
  );
};

export { SchoolProfiles };
