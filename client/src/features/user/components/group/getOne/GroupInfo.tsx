import { FC } from 'react';
import type { OneGroupResponse } from '@/features/user/types/groupTypes';
import { Table } from 'flowbite-react';
import { DateTime } from '@/components/date-time';
import { useTranslations } from 'next-intl';

type GroupInfoProps = {
  group: OneGroupResponse
}

const GroupInfo: FC<GroupInfoProps> = ({ group }) => {
  const t = useTranslations('Group')

  return (
    <Table>
      <Table.Body className="divide-y">
        <Table.Row>
          <Table.Cell className="w-1/2 bg-purple-100 px-4 py-2 font-semibold text-center">
            {t('labels.name')}
          </Table.Cell>
          <Table.Cell className="w-1/2 px-4 py-2 font-medium text-center">
            {group.name}
          </Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell className="w-1/2 px-4 py-2 font-semibold text-center bg-purple-100">
            {t('labels.studyPeriodNumber')}
          </Table.Cell>
          <Table.Cell className="w-1/2 px-4 py-2 font-medium text-center">
            {group.studyPeriodNumber}
          </Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell className="w-1/2 px-4 py-2 font-semibold text-center bg-purple-100">
            {t('labels.createdAt')}
          </Table.Cell>
          <Table.Cell className="w-1/2 px-4 py-2 font-medium text-center">
            <DateTime date={group.createdAt} />
          </Table.Cell>
        </Table.Row>
        {group.lastUpdatedAt &&
          <Table.Row>
            <Table.Cell className="w-1/2 px-4 py-2 font-semibold text-center bg-purple-100">
              {t('labels.lastUpdatedAt')}
            </Table.Cell>
            <Table.Cell className="w-1/2 px-4 py-2 font-medium text-center">
              <DateTime date={group.lastUpdatedAt} />
            </Table.Cell>
          </Table.Row>
        }
        <Table.Row>
          <Table.Cell className="w-1/2 px-4 py-2 bg-purple-100 font-semibold text-center">
            {t('labels.id')}
          </Table.Cell>
          <Table.Cell className="w-1/2 px-4 py-2 text-xs text-gray-400 font-medium text-center">
            {group.id}
          </Table.Cell>
        </Table.Row>
      </Table.Body>
    </Table>
  );
};

export { GroupInfo };
