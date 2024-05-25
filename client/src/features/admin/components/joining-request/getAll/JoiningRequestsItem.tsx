import { FC } from 'react';
import { JoiningRequestResponse } from '../../../types/joiningRequestTypes';
import { Table, Button } from 'flowbite-react';
import { useMediaQuery } from 'react-responsive';
import { useLocale, useTranslations } from 'next-intl';
import { getColorByStatus } from '@/shared/helpers';
import Link from 'next/link';
import { AdminClientPaths } from '@/features/admin/constants';
import { mediaQueries } from '@/shared/constants';
import { DateTime } from '@/components/date-time';

interface JoiningRequestsItemProps {
  item: JoiningRequestResponse;
  index: number;
}

const JoiningRequestsItem: FC<JoiningRequestsItemProps> = ({ item, index }) => {
  const activeLocale = useLocale();
  const t = useTranslations('JoiningRequest');
  const isDesktopOrLaptop = useMediaQuery({
    query: mediaQueries.desktopOrLaptop,
  });

  const statusColor = getColorByStatus(item.status);

  return (
    <>
      {isDesktopOrLaptop ? (
        <Table.Row className="bg-white font-medium dark:border-gray-700 dark:bg-gray-800">
          <Table.Cell>{item.name}</Table.Cell>
          <Table.Cell>{item.requesterFullName}</Table.Cell>
          <Table.Cell>{item.requesterEmail}</Table.Cell>
          <Table.Cell>{item.requesterPhone}</Table.Cell>
          <Table.Cell>
            {item.region === 'none' ? '-' : t(`regions.${item.region}`)}
          </Table.Cell>
          <Table.Cell>
            <DateTime date={item.createdAt} />
          </Table.Cell>
          <Table.Cell>
            <span className={`${statusColor} font-bold`}>
              {t(`labels.status.${item.status}`)}
            </span>
          </Table.Cell>
          <Table.Cell>
            <Button gradientMonochrome="purple">
              <Link
                href={`/${activeLocale}/${AdminClientPaths.JoiningRequests}/${item.id}`}
                className="text-white"
              >
                {t('labels.details')}
              </Link>
            </Button>
          </Table.Cell>
        </Table.Row>
      ) : (
        <>
          <h6 className="py-2 text-center text-lg font-bold">
            <span className="color-gray-500 mr-1 text-sm font-normal">
              # {index + 1}
            </span>
            {item.name}
          </h6>

          <div className="flex">
            <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 text-xs font-semibold">
              <span className="text-center">{t('labels.status.label')}</span>
            </div>
            <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 text-xs font-medium">
              <span className={`${statusColor} font-bold`}>
                {t(`labels.status.${item.status}`)}
              </span>
            </div>
          </div>

          <div className="flex">
            <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 text-xs font-semibold">
              <span className="text-center">{t('labels.createdAt')}</span>
            </div>
            <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 text-xs font-medium">
              <span>
                <DateTime date={item.createdAt} />
              </span>
            </div>
          </div>

          <div className="flex">
            <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 text-xs font-semibold">
              <span className="text-center">
                {t('labels.requesterFullName')}
              </span>
            </div>
            <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 text-xs font-medium">
              {item.requesterFullName}
            </div>
          </div>

          <div className="flex">
            <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 text-xs font-semibold">
              <span className="text-center">{t('labels.requesterEmail')}</span>
            </div>
            <div className="flex w-1/2 justify-center whitespace-normal border border-gray-200 px-4 py-2 text-xs font-medium">
              {item.requesterEmail}
            </div>
          </div>

          <div className="flex">
            <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 text-xs font-semibold">
              <span className="text-center">{t('labels.requesterPhone')}</span>
            </div>
            <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 text-xs font-medium">
              {item.requesterPhone}
            </div>
          </div>

          <div className="flex">
            <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 text-xs font-semibold">
              <span className="text-center">{t('labels.region')}</span>
            </div>
            <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 text-xs font-medium">
              {item.region === 'none' ? '-' : t(`regions.${item.region}`)}
            </div>
          </div>

          <div className="mt-2 flex">
            <Button gradientMonochrome="purple" fullSized>
              <Link
                href={`/${activeLocale}/${AdminClientPaths.JoiningRequests}/${item.id}`}
                className="w-[100%] text-white"
              >
                {t('labels.details')}
              </Link>
            </Button>
          </div>
          <hr
            className="mx-auto mb-4 h-[3px] rounded border-0 bg-gray-200
          dark:bg-gray-700 md:my-5"
          />
        </>
      )}
    </>
  );
};

export { JoiningRequestsItem };
