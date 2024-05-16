import { JoiningRequests } from '@/features/admin';

type Props = {
  searchParams: {
    region: string,
    sortByDateAsc: string,
    schoolName: string
  }
}

export default function JoiningRequestsPage({ searchParams }: Props) {
  return (
    <div className="p-3">
      <JoiningRequests
        sortByDateAscParameter={searchParams.sortByDateAsc}
        regionParameter={searchParams.region}
        searchParameter={searchParams.schoolName}
      />
    </div>
  );
}
