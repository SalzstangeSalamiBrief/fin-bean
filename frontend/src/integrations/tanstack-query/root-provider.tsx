import { QueryClient } from '@tanstack/react-query';

// oxlint-disable-next-line react/only-export-components
export function getContext() {
  const queryClient = new QueryClient();

  return {
    queryClient,
  };
}
export default function TanstackQueryProvider() {}
