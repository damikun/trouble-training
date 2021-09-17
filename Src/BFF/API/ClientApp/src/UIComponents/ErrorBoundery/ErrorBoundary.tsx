import React, { ReactNode, useContext } from "react";

// Do not catch errors for:
// Event handlers
// Asynchronous code
// Server-side rendering
// Errors are thrown in the error boundary itself (rather than its children)

export type ErrorBoundaryProps = {
  children: ReactNode;
  fallback?: NonNullable<ReactNode> | null;
};

export type ErrorBoundaryState = {
  hasError: boolean;
};

export type BoundaryContextType = {
  reset(): void;
};

export const BoundaryContext = React.createContext<BoundaryContextType>({
  reset: () => {},
});

export const useBoundaryContext = () => useContext(BoundaryContext);

export default class ErrorBoundary extends React.Component<
  ErrorBoundaryProps,
  ErrorBoundaryState
> {
  constructor(props: ErrorBoundaryProps) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error: any) {
    return { hasError: true };
  }

  componentDidCatch(error: any, errorInfo: any) {
    console.log(error, errorInfo);
  }

  reset() {
    this.setState({ hasError: false });
  }

  render() {
    return (
      <BoundaryContext.Provider
        value={{
          reset: () => {
            this.reset();
          },
        }}
      >
        {this.state.hasError ? this.props.fallback : this.props.children}
      </BoundaryContext.Provider>
    );
  }
}
