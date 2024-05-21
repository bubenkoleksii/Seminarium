import { FC } from 'react';
import type { SchoolResponse } from '@/features/admin/types/schoolTypes';
import { useLocale, useTranslations } from 'next-intl';
import { mediaQueries } from '@/shared/constants';
import { useMediaQuery } from 'react-responsive';
import { Button, Table } from 'flowbite-react';
import { DateTime } from '@/components/date-time';
import { AdminClientPaths } from '@/features/admin/constants';
import Link from 'next/link';

interface SchoolItemProps {
  item: SchoolResponse;
  index: number;
}

const SchoolItem: FC<SchoolItemProps> = ({ item, index }) => {
  const activeLocale = useLocale();
  const t = useTranslations('School');

  const isDesktopOrLaptop = useMediaQuery({
    query: mediaQueries.desktopOrLaptop,
  });

  return (
    <>
      {isDesktopOrLaptop ? (
        <Table.Row className="bg-white font-medium dark:border-gray-700 dark:bg-gray-800">
          <Table.Cell>{item.name}</Table.Cell>
          <Table.Cell>{item.registerCode}</Table.Cell>
          <Table.Cell>{item.studentsQuantity}</Table.Cell>
          <Table.Cell>{t(`types.${item.type}`)}</Table.Cell>
          <Table.Cell>{t(`ownershipTypes.${item.ownershipType}`)}</Table.Cell>
          <Table.Cell>
            {item.region === 'none' ? '-' : t(`regions.${item.region}`)}
          </Table.Cell>
          <Table.Cell>
            <DateTime date={item.createdAt} />
          </Table.Cell>
          <Table.Cell>
            <Button gradientMonochrome="purple">
              <Link
                href={`/${activeLocale}/${AdminClientPaths.Schools}/${item.id}`}
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
                {t('labels.registerCode')}
              </span>
            </div>
            <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 text-xs font-medium">
              {item.registerCode}
            </div>
          </div>

          <div className="flex">
            <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 text-xs font-semibold">
              <span className="text-center">{t('labels.studentsQuantity')}</span>
            </div>
            <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 text-xs font-medium">
              {item.studentsQuantity}
            </div>
          </div>

          <div className="flex">
            <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 text-xs font-semibold">
              <span className="text-center">{t('labels.type')}</span>
            </div>
            <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 text-xs font-medium">
              {t(`types.${item.type}`)}
            </div>
          </div>

          <div className="flex">
            <div className="flex w-1/2 justify-center border border-gray-200 bg-purple-100 px-4 py-2 text-xs font-semibold">
              <span className="text-center">{t('labels.ownershipType')}</span>
            </div>
            <div className="flex w-1/2 justify-center border border-gray-200 px-4 py-2 text-xs font-medium">
              {t(`ownershipTypes.${item.ownershipType}`)}
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

export { SchoolItem };
