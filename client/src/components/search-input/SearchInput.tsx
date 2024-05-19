import { ChangeEvent, FC, FormEvent, useState } from 'react';
import { useTranslations } from 'next-intl';
import { X } from 'lucide-react';

interface SearchInputProps {
  placeholder?: string;
  value?: string;
  onSubmit: (searchValue: string) => void;
  size?: 'small' | 'medium' | 'large';
  maxLength?: number;
}

const SearchInput: FC<SearchInputProps> = ({
  placeholder,
  value,
  onSubmit,
  maxLength,
  size = 'medium',
}) => {
  const t = useTranslations('SearchInput');
  const [searchValue, setSearchValue] = useState<string>(value || '');

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setSearchValue(value);

    if (!value) onSubmit('');
  };

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    onSubmit(searchValue);
  };

  const handleClear = () => {
    setSearchValue('');

    onSubmit('');
  };

  const getSizeClassName = () => {
    switch (size) {
      case 'small':
        return 'px-3 py-1 text-sm';
      case 'large':
        return 'px-6 py-3 text-lg';
      default:
        return 'px-4 py-2 text-base';
    }
  };

  return (
    <form onSubmit={handleSubmit} className="mx-auto max-w-xl">
      <div className="relative">
        <div className="pointer-events-none absolute inset-y-0 start-0 flex items-center ps-3">
          <svg
            className="h-4 w-4 text-gray-500 dark:text-gray-400"
            aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 20 20"
          >
            <path
              stroke="currentColor"
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth="2"
              d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
            />
          </svg>
        </div>
        <input
          type="search"
          id="default-search"
          maxLength={maxLength}
          className={`block h-[55px] w-full rounded-lg border border-gray-300 bg-gray-50 p-4 
            ps-10 text-gray-900 focus:border-purple-950 focus:ring-purple-950 dark:bg-gray-700 
            ${getSizeClassName()}`}
          placeholder={placeholder || ''}
          value={searchValue}
          onChange={handleInputChange}
          required
        />
        <div
          onClick={handleClear}
          className={`absolute bottom-4 end-24 cursor-pointer`}
        >
          <X color="#6b6b77" />
        </div>
        <button
          type="submit"
          className={`absolute bottom-2 end-2.5 rounded-lg bg-purple-950 p-2 
            font-medium text-white hover:bg-purple-900 focus:outline-none focus:ring-blue-300`}
        >
          {t('label')}
        </button>
      </div>
    </form>
  );
};

export { SearchInput };
