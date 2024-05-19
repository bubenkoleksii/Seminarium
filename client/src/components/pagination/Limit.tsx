import { FC } from 'react';
import { useTranslations } from 'next-intl';

interface LimitProps {
  limitOptions: number[];
  currentLimit: number;
  onChangeLimit: (limit: number) => void;
}

const Limit: FC<LimitProps> = ({
  limitOptions,
  currentLimit,
  onChangeLimit,
}) => {
  const t = useTranslations('Pagination');

  return (
    <div className="flex w-[300px] flex-col items-center">
      <label className="relative mb-1 block text-center font-medium text-gray-700">
        {t('limit')}
      </label>

      <div className="flex">
        {limitOptions.map((option) => (
          <button
            key={option}
            className={
              option === currentLimit
                ? 'mx-2 cursor-pointer rounded bg-purple-950 px-4 py-2 text-white'
                : 'mx-2 cursor-pointer rounded bg-white px-4 py-2 text-gray-800 hover:bg-gray-200'
            }
            onClick={() => onChangeLimit(option)}
          >
            {option}
          </button>
        ))}
      </div>
    </div>
  );
};

export { Limit };
