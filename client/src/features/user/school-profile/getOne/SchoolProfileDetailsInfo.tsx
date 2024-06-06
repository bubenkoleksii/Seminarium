import { FC } from 'react';
import { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { useTranslations } from 'next-intl';
import { Table } from 'flowbite-react';
import { DateOnly } from '@/components/date-time';

type SchoolProfileDetailsInfoProps = {
  profile: SchoolProfileResponse
}

const SchoolProfileDetailsInfo: FC<SchoolProfileDetailsInfoProps> = ({ profile }) => {
  const t = useTranslations('SchoolProfile');

  const renderRow = (label: string, value: any) => (
    <Table.Row key={label}>
      <Table.Cell className="w-1/2 bg-purple-100 px-4 py-2 text-center font-semibold">
        {t(label)}
      </Table.Cell>
      <Table.Cell className="w-1/2 px-4 py-2 text-center font-medium">
        {value
          ? (label.includes('Date') || label.includes('At') ? <DateOnly date={value} /> : value)
          : '-'
        }
      </Table.Cell>
    </Table.Row>
  );

  return (
    <div className="px-4 flex justify-center mt-3">
      <Table className="w-full max-w-full sm:max-w-3/4 md:max-w-2/3 lg:max-w-1/2">
        <Table.Body>
          {renderRow('item.name', profile.name)}
          {profile.type !== 'parent' && renderRow('item.school', profile.schoolName)}
          {renderRow('item.createdAt', profile.createdAt)}
          {renderRow('item.lastUpdatedAt', profile.lastUpdatedAt)}
          {renderRow('item.type', t(`type.${profile.type}`))}
          {renderRow('item.phone', profile.phone)}
          {renderRow('item.email', profile.email)}
          {renderRow('item.details', profile.details)}

          {profile.type === 'teacher' && (
            <>
              {renderRow('item.teacherSubjects', profile.teacherSubjects)}
              {renderRow('item.teacherExperience', profile.teacherExperience)}
              {renderRow('item.teacherEducation', profile.teacherEducation)}
              {renderRow('item.teacherQualification', profile.teacherQualification)}
              {renderRow('item.teacherLessonsPerCycle', profile.teacherLessonsPerCycle)}
            </>
          )}

          {profile.type === 'student' && (
            <>
              {renderRow('item.studentHealthGroup', profile.studentHealthGroup ? t(`item.healthGroup.${profile.studentHealthGroup}`) : '-')}
              {renderRow('item.studentDateOfBirth', profile.studentDateOfBirth)}
              {renderRow('item.studentAptitudes', profile.studentAptitudes)}
              {renderRow('item.studentIsClassLeader', profile.studentIsClassLeader ? t('yes') : t('no'))}
              {renderRow('item.studentIsIndividually', profile.studentIsIndividually ? t('yes') : t('no'))}
            </>
          )}

          {profile.type === 'parent' && (
            <>
              {renderRow('item.parentAddress', profile.parentAddress)}
            </>
          )}
        </Table.Body>
      </Table>
    </div>
  );
};

export default SchoolProfileDetailsInfo;
