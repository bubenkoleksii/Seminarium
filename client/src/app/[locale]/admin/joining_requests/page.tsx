import { JoiningRequests } from '@/features/admin';

type Props = {
  searchParams: {
    region: string,
    sortByDateAsc: string,
    schoolName: string,
    take: string,
    page: string,
  }
}

export default function JoiningRequestsPage({ searchParams }: Props) {
  return (
    <div className="p-3">
      <JoiningRequests
        sortByDateAscParameter={searchParams.sortByDateAsc}
        regionParameter={searchParams.region}
        searchParameter={searchParams.schoolName}
        limitParameter={Number(searchParams.take)}
        pageParameter={Number(searchParams.page)}
      />
    </div>
  );
}
