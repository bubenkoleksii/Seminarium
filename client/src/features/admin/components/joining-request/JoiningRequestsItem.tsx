import { FC } from 'react';
import { JoiningRequestResponse } from '../../types/joiningRequestTypes';
import { Table, Button } from 'flowbite-react';
import { useMediaQuery } from 'react-responsive';
import { useLocale, useTranslations } from 'next-intl';
import { getColorByStatus } from '@/shared/helpers';
import Link from 'next/link';
import { AdminClientPaths } from '@/features/admin/constants';

interface JoiningRequestsItemProps {
  item: JoiningRequestResponse;
  index: number;
}

const JoiningRequestsItem: FC<JoiningRequestsItemProps> = ({ item, index }) => {
  const activeLocale = useLocale();
  const t = useTranslations('JoiningRequest');
  const isDesktopOrLaptop = useMediaQuery({ query: '(min-width: 1280px)' });

  const statusColor = getColorByStatus(item.status);
  return (
    <>
      {isDesktopOrLaptop ? (
        <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800 font-medium">
          <Table.Cell>{item.name}</Table.Cell>
          <Table.Cell>{item.requesterFullName}</Table.Cell>
          <Table.Cell>{item.requesterEmail}</Table.Cell>
          <Table.Cell>{item.requesterPhone}</Table.Cell>
          <Table.Cell>{t(`regions.${item.region}`)}</Table.Cell>
          <Table.Cell>
            <span className={`${statusColor} font-bold`}>
              {t(`labels.status.${item.status}`)}
            </span>
          </Table.Cell>
          <Table.Cell>
            <Table.Cell>
              <Button gradientMonochrome="purple">
                <Link href={`/${activeLocale}/${AdminClientPaths.JoiningRequests}/${item.id}`}
                      className="text-white"
                >
                  {t('labels.details')}
                </Link>
              </Button>
            </Table.Cell>
          </Table.Cell>
        </Table.Row>
      ) : (
        <>
          <h6 className="py-2 font-bold text-lg text-center">
            <span className="text-sm color-gray-500 font-normal mr-1"># {index + 1}</span>
            {item.name}
          </h6>
          <div className="flex">
            <div className="w-1/2 text-xs font-semibold bg-purple-100 flex justify-center py-2 px-4 border border-gray-200">
              <span className="text-center">{t('labels.status.label')}</span>
            </div>
            <div className="w-1/2 text-xs font-medium flex justify-center py-2 px-4 border border-gray-200">
              <span className={`${statusColor} font-bold`}>
                {t(`labels.status.${item.status}`)}
              </span>
            </div>
          </div>
          <div className="flex">
            <div className="w-1/2 text-xs font-semibold bg-purple-100 flex justify-center py-2 px-4 border border-gray-200">
              <span className="text-center">{t('labels.requesterFullName')}</span>
            </div>
            <div className="w-1/2 text-xs font-medium flex justify-center py-2 px-4 border border-gray-200">
              {item.requesterFullName}
            </div>
          </div>
          <div className="flex">
            <div className="w-1/2 text-xs font-semibold bg-purple-100 flex justify-center py-2 px-4 border border-gray-200">
              <span className="text-center">{t('labels.requesterEmail')}</span>
            </div>
            <div className="w-1/2 text-xs whitespace-normal font-medium flex justify-center py-2 px-4 border border-gray-200">
              {item.requesterEmail}
            </div>
          </div>
          <div className="flex">
            <div className="w-1/2 text-xs font-semibold bg-purple-100 flex justify-center py-2 px-4 border border-gray-200">
              <span className="text-center">{t('labels.requesterPhone')}</span>
            </div>
            <div className="w-1/2 text-xs font-medium flex justify-center py-2 px-4 border border-gray-200">
              {item.requesterPhone}
            </div>
          </div>
          <div className="flex">
            <div className="w-1/2 text-xs font-semibold bg-purple-100 flex justify-center py-2 px-4 border border-gray-200">
              <span className="text-center">{t('labels.region')}</span>
            </div>
            <div className="w-1/2 text-xs font-medium flex justify-center py-2 px-4 border border-gray-200">
              {t(`regions.${item.region}`)}
            </div>
          </div>
          <div className="flex mt-2">
            <Button gradientMonochrome="purple" fullSized>
              <Link href={`/${activeLocale}/${AdminClientPaths.JoiningRequests}/${item.id}`}
                    className="text-white"
              >
                {t('labels.details')}
              </Link>
            </Button>
          </div>
          <hr className="mx-auto mb-4 h-[3px] rounded border-0 bg-gray-200 dark:bg-gray-700 md:my-5" />
        </>
      )}
    </>
  );
};

export { JoiningRequestsItem };
