import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  async rewrites() {
    return [
      {
        source: "/backend/:path*",
        destination: "http://titanuem.runasp.net/:path*",
      },
    ];
  },
};

export default nextConfig;
